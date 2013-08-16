module Testgen.Parser
open FParsec
open FParsec.CharParsers
open FParsec.Primitives

// AST
type Exprs = Exp list
and Exp =
  | IDVal      of string
  | Op         of string * Exp * Exp 
  | Fail       of string


// Parser
let ws = spaces
let ch c = skipChar c >>. ws

// Term
let symbol: Parser<string, unit> =
  parse {
    let! h = letter
    let! t = manyChars (letter <|> digit <|> anyOf "-_")
    return System.Char.ToString(h) + t
    }

let termParse : Parser<Exp, unit> =
   parse { 
     let! wd = symbol
     return (IDVal wd)
     } 


//	OPP
let opp = new OperatorPrecedenceParser<Exp, unit, unit>()
let expr = opp.ExpressionParser
let term = (termParse .>> ws) <|> between (ch '(') (ch ')') expr
opp.TermParser <- term
opp.AddOperator(InfixOperator("=", ws, 
                         2, 
                         Associativity.Left, 
                         fun x y -> (Op ("=" , x , y))))
opp.AddOperator(InfixOperator("+", ws, 
                         5, 
                         Associativity.Left, 
                         fun x y -> (Op ("+" , x , y)))) 
opp.AddOperator(InfixOperator("*", ws, 
                         6, 
                         Associativity.Left, 
                         fun x y -> (Op ("*" , x , y))))
opp.AddOperator(InfixOperator("<-", ws, 
                         6, 
                         Associativity.Left, 
                         fun x y -> (Op ("<-" , x , y))))


let mainParser: Parser<Exp, unit> =
  parse { 
    let! ast = expr
    let! _ = manyChars (anyOf ";")
    return ast
    }

let cm: Parser<Exp list, unit> =
  parse { 
    do! spaces
    let! ast = sepEndBy mainParser spaces
    do! eof
    return ast
    }

    //
// ParserResult<Exp list> から、Exp list を取り出す
let extractExprs x =
  match x with
  | Success (x, _, _) -> x
  | Failure (x,y,_) -> failwith x


let testParse prg = 
  ((run cm) prg) |> extractExprs