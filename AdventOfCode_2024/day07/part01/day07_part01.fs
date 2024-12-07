module day07_part01

open AdventOfCode_2024.Modules

let parseContent(lines: string array) =
    lines
    |> Array.map(fun line ->
        (
            bigint.Parse(line.Split(":")[0]),
            (line.Split(":")[1]).Trim().Split(" ")
                |> Array.map bigint.Parse)
    )

let compute(expr: bigint*bigint array) =
    let rec calculate(expected: bigint) (tocalculate: bigint list) (currentValue: bigint)=
        if currentValue > expected then false
        else
            match tocalculate with
            | [] -> expected = currentValue
            | newvalue :: tocompute ->
                (calculate (expected) (tocompute) (newvalue * currentValue)) ||
                (calculate expected tocompute (newvalue + currentValue))

    calculate (fst expr) ((snd expr) |> List.ofArray) 0I
        

let execute() =
    let path = "day07/day07_input.txt"
    let content = LocalHelper.GetLinesFromFile path

    parseContent content
        |> Array.filter compute
        |> Array.sumBy fst
