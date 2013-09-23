module Testgen.Parser
open FParsec
open FParsec.CharParsers
open FParsec.Primitives

// AST
type Exprs = Exp list
and Exp =
  | Decl       of string * string list
  | IDVal      of string
  | Op         of string * Exp * Exp 
  | Fail       of string


// white space parser
let private ws = spaces
let ch c = skipChar c >>. ws

// symbol parser
let private symbol: Parser<string, unit> =
  parse {
    let! h = letter
    let! t = manyChars (letter <|> digit <|> anyOf "-_")
    return System.Char.ToString(h) + t
    }

let private term : Parser<Exp, unit> =
   parse { 
     let! wd = symbol
     return (IDVal wd)
     } 


//	operator
let private opp = new OperatorPrecedenceParser<Exp, unit, unit>()
let private expression = opp.ExpressionParser
opp.TermParser <- (term .>> ws) <|> between (ch '(') (ch ')') expression
opp.AddOperator(InfixOperator("=", ws, 2, Associativity.Left, fun x y -> (Op ("=" , x , y))))
opp.AddOperator(InfixOperator("+", ws, 5, Associativity.Left, fun x y -> (Op ("+" , x , y)))) 
opp.AddOperator(InfixOperator("*", ws, 6, Associativity.Left, fun x y -> (Op ("*" , x , y))))
opp.AddOperator(InfixOperator("<-", ws, 6, Associativity.Left, fun x y -> (Op ("<-" , x , y))))

// declaration
let private element: Parser<string, unit> =
  parse {
  let!  t = manyChars (letter <|> digit <|> anyOf "-_")
  return t
  }

let private elements: Parser<string list, unit> =
  parse {
  do! spaces
  let! elems = sepBy element (ch ',')
  return elems
  }

let private declaration: Parser<Exp, unit> = 
  parse {
  let! wd = symbol
  do! ch ':'
  let! elems = elements
  return (Decl(wd, elems))
  }

let private tryParse p: Parser<_, _> =
  parse {
  let! _ = lookAhead p
  let! wd = p
  return wd
  }

let private ast: Parser<Exp, unit> =
  parse {
  let! dec = tryParse declaration
  return dec
  }
  <|> parse {
  let! exp = tryParse expression
  return exp
  }

let private rows: Parser<Exp, unit> =
  parse { 
    let! ret = ast
    let! _ = manyChars (anyOf ";")
    return ret
    }

let private main: Parser<Exp list, unit> =
  parse { 
    do! spaces
    let! ret = sepEndBy rows spaces
    do! eof
    return ret
    }


// ParserResult<Exp list> から、Exp list を取り出す
let private extractExprs x =
  match x with
  | Success (x, _, _) -> x
  | Failure (x,y,_) -> failwith x

[<CompiledName "ParseText">]
let parseText prg = ((run main) prg) |> extractExprs