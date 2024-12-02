module day02_part02

open AdventOfCode_2024.Modules


let parseContent (lines: string array) =
    lines
    |> Array.map(fun line -> line.Split(" ") |> Array.map int)

let getExclusions (arr: 'a array) : 'a array list =
    [for i in 0 .. arr.Length - 1 -> Array.append (arr.[0 .. i-1]) (arr.[i+1 ..])]

let checkWithoutOne(elems: int array) =
    let areSafeInc(tocheck: int array) =
        tocheck
        |> Array.pairwise
        |> Array.forall (fun (a, b) -> b - a >= 1 && b - a <= 3)
    let areSafeDec(tocheck: int array) =
        tocheck
        |> Array.pairwise
        |> Array.forall (fun (a, b) -> a - b >= 1 && a - b <= 3)  

    let isSafe e = areSafeInc e || areSafeDec e
    getExclusions elems
    |> List.exists isSafe

let execute =
    let path = "day02/day02_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let values = parseContent content
    values
    |> Array.filter checkWithoutOne
    |> Array.length
