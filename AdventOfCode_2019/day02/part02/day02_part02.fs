module day02_part02

open System.IO

let (height, width) = (100, 100)
let __SOLUTION__ = 19690720

let solutionMatrix = 
    seq {
        for row in 0 .. width - 1 do
            for col in 0 .. height - 1 ->
                (row, col)
        }

let rec performOperation (values: int[]) (idx: int) =
    if idx < values.Length then
        match values.[idx] with
            | 1 -> 
                Array.set values values.[idx + 3] (values.[values.[idx + 1]] + values.[values.[idx + 2]])
                performOperation values (idx + 4)
            | 2 -> 
                Array.set values values.[idx + 3] (values.[values.[idx + 1]] * values.[values.[idx + 2]])
                performOperation values (idx + 4)
            | 99 -> idx
            | _ -> idx
    else
       idx

let displaySolutionMatrix =
    solutionMatrix
        |> Seq.skipWhile (fun (noum, verb) -> 
            let filepath = __SOURCE_DIRECTORY__ + @"../../day02_input.txt"
            //let filepath = __SOURCE_DIRECTORY__ + @"../../test_input_yavuz.txt"
            let values = File.ReadAllText(filepath).Split(',')
                            |> Array.map int
            
            // prepare the tranche input 
            Array.set values 1 noum
            Array.set values 2 verb

            let calculated = performOperation values 0
            values.[0] <> __SOLUTION__
        )
        |> Seq.head 

let execute =   
    match displaySolutionMatrix with
        | (noum, verb) -> noum * 100 + verb