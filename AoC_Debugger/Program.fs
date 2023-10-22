open System
open System.Text.Json
open System.Collections.Generic

let path = "./test_input.txt"

let input = System.IO.File.ReadLines path |> Seq.map int |> Seq.toArray

let rec execute (input: int[]) (index: int) (steps: int) =
    let length = input.Length
    if index < 0 || index >= length then
        steps
    else
        let jump = input.[index]
        let newIndex = index + jump
        input.[index] <- input.[index] + 1
        execute input newIndex (steps + 1)

let result = execute input 0 0
printfn "Number of jumps : %i" result