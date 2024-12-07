module day07_part01

open AdventOfCode_2024.Modules

let parseContent(lines: string array) =
    lines
    |> Array.map(fun line ->
        (
           System.Int64.Parse(line.Split(":")[0]),
            (line.Split(":")[1]).Trim().Split(" ")
                |> Array.map int64)
    )

let compute((expected, values): int64*int64 array) (ops: (int64->int64->int64) list)=
    let rec calculate(expected': int64) (tocalculate: int64 list) (currentValue: int64)=
        if currentValue > expected' then false
        else
            match tocalculate with
            | [] -> expected' = currentValue
            | newvalue :: tocompute ->
                ops
                |> List.exists(fun op -> (calculate (expected') (tocompute) (op newvalue currentValue)))

    calculate expected (values |> List.ofArray) 0
        

let execute() =
    let path = "day07/day07_input.txt"
    let content = LocalHelper.GetLinesFromFile path

    parseContent content
    |> Array.sumBy (fun (expected, values) -> 
        if compute (expected, values) [(+); (*)] then expected else 0
    )