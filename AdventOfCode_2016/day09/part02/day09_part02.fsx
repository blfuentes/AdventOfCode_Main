open System.Text

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System.Collections.Generic
open System.Text.RegularExpressions
open AdventOfCode_Utilities
open AdventOfCode_2016.Modules
open System.Numerics

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
    let regex = Regex(@"\((?<size>\d+)x(?<times>\d+)\)")
    let memo = Dictionary<int, BigInteger>()

    let rec dp (content: string) (idx: int) =
        if idx >= content.Length then BigInteger.Zero
        else
            match memo.TryGetValue(idx) with
            | true, v -> v
            | false, _ ->
                let found = regex.Match(content, idx)
                if found.Success && found.Index = idx then
                    let size = BigInteger.Parse(found.Groups.["size"].Value)
                    let times = BigInteger.Parse(found.Groups.["times"].Value)
                    let startIdx = found.Index + found.Length
                    let endIdx = startIdx + int size
                    let segment = content.Substring(startIdx, int size)
                    let repeatedSegmentLength = times * (dp segment 0)
                    let result = repeatedSegmentLength + (dp content endIdx)
                    memo.[idx] <- result
                    result
                else
                    let result = BigInteger.One + (dp content (idx + 1))
                    memo.[idx] <- result
                    result

    dp content 0



let processContent (content: string) =
    let regex = Regex(@"\((?<size>\d+)x(?<times>\d+)\)")

    let rec dp (content: string) (memo: Dictionary<string, int>) =
        if memo.ContainsKey(content) then
            memo.[content]
        else
            let mutable index = 0
            let mutable totalLength = 0
            while index < content.Length do
                let found = regex.Match(content, index)
                if found.Success && found.Index = index then
                    let size = int found.Groups.["size"].Value
                    let times = int found.Groups.["times"].Value
                    let startIdx = found.Index + found.Length
                    let endIdx = startIdx + size
                    let segment = content.Substring(startIdx, size)
                    totalLength <- totalLength + times * (dp segment memo)
                    index <- endIdx
                else
                    totalLength <- totalLength + 1
                    index <- index + 1
            memo.[content] <- totalLength
            totalLength

    let memo = Dictionary<string, int>()
    dp content memo




    

let content = "ADVENT"
let content = "A(1x5)BC"
let content = "(3x3)XYZ"
let content = "A(2x2)BCD(2x2)EFG"
let content = "(6x1)(1x3)A"
let content = "X(8x2)(3x3)ABCY"
let content: string = "(27x12)(20x12)(13x14)(7x10)(1x12)A"
let content = "(25x3)(3x3)ABC(2x3)XY(5x2)PQRSTX(18x9)(3x2)TWO(5x7)SEVEN"


(processContent content)