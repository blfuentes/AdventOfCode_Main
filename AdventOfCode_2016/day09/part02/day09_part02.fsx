open System.Text

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System.Collections.Generic
open System.Text.RegularExpressions
open AdventOfCode_Utilities
open AdventOfCode_2016.Modules

//let path = "day09/test_input_01.txt"
let path = "day09/day09_input.txt"

let content = (LocalHelper.GetContentFromFile path)

let rec processContent (content: string) (currentIdx: int) =
    let regex = new Regex("\((?<size>\d+)x(?<times>\d+)\)")
    let found = regex.Match( content)
    match found.Success with
    | true ->
        let endIdx = (found.Value.Length + found.Index)
        let substring = content.Substring(found.Index, found.Value.Length)
        printfn "starts at %i and ends at %i: %s" found.Index endIdx substring
        let size = (int)found.Groups["size"].Value
        let times = (int)found.Groups["times"].Value
        let toRepeat = content.Substring(endIdx, size)
        let repeatedContent = toRepeat |> String.replicate(times)
        //printfn "newContent: %s" repeatedContent
        let newContent = content.Substring(0, found.Index) + repeatedContent + content.Substring(endIdx + size)
        //printfn "finalContent: %s" newContent
        processContent newContent (currentIdx + size * times)
    | false ->           
        content.Length

let processContent (content: string) =
    let memo = Dictionary<int, int>()
    let regex = Regex(@"\((?<size>\d+)x(?<times>\d+)\)")

    let rec dp (idx: int) =
        if idx >= content.Length then 0
        else
            match memo.TryGetValue(idx) with
            | true, v -> v
            | false, _ ->
                let found = regex.Match(content, idx)
                if found.Success && found.Index = idx then
                    let size = int found.Groups.["size"].Value
                    let times = int found.Groups.["times"].Value
                    let endIdx = found.Index + found.Length
                    let repeatedLen = size * times
                    let result = repeatedLen + dp (endIdx + size)
                    memo.[idx] <- result
                    result
                else
                    memo.[idx] <- 1 + dp (idx + 1)
                    memo.[idx]
    dp 0

    

let content = "ADVENT"
let content = "A(1x5)BC"
let content = "(3x3)XYZ"
let content = "A(2x2)BCD(2x2)EFG"
let content = "(6x1)(1x3)A"
let content = "X(8x2)(3x3)ABCY"
let content = "(27x12)(20x12)(13x14)(7x10)(1x12)A"
let content = "(25x3)(3x3)ABC(2x3)XY(5x2)PQRSTX(18x9)(3x2)TWO(5x7)SEVEN"


processContent content