module day19_part02

open AdventOfCode_2024.Modules
open System.Collections.Generic

let parseContent(lines: string) =
    let patterns = (lines.Split("\r\n\r\n")[0]).Split(", ")
    let designs = (lines.Split("\r\n\r\n")[1]).Split("\r\n")
    (patterns, designs)

let checkIfValid (design: string) (patterns: Set<string>) maxSize (memo: Dictionary<string, int64>) =   
    let rec canBeDone (patterns: Set<string>) foundSoFar designpart =
        if memo.ContainsKey designpart then
            memo[designpart]
        else
            if designpart = "" then foundSoFar + 1L
            else
                let minlength = min maxSize designpart.Length
                let mutable numOfFounds = 0L
                for l in 0..minlength do
                    let subpart = designpart.Substring(0, l)
                    if Set.contains subpart patterns then
                        numOfFounds <- numOfFounds + canBeDone patterns foundSoFar (designpart.Substring(subpart.Length))

                memo.Add(designpart, numOfFounds) |> ignore
                numOfFounds

    canBeDone patterns 0 design

let execute() =
    let path = "day19/day19_input.txt"
    let content = LocalHelper.GetContentFromFile path
    let (patterns, designs) = parseContent content
    let maxPatternSize = ((patterns |> Array.sortByDescending _.Length)[0]).Length
    let memo = Dictionary<string, int64>()
    designs
    |> Array.sumBy(fun d ->
        checkIfValid d (patterns |> Set.ofArray) maxPatternSize memo
    )
