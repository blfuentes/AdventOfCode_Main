module day19_part01

open AdventOfCode_2024.Modules
open System.Collections.Generic

let parseContent(lines: string) =
    let patterns = (lines.Split("\r\n\r\n")[0]).Split(", ")
    let designs = (lines.Split("\r\n\r\n")[1]).Split("\r\n")
    (patterns, designs)

let checkIfValid (design: string) (patterns: Set<string>) maxSize (memo: Dictionary<string, bool>) =   
    let rec canBeDone (patterns: Set<string>) designpart =
        if memo.ContainsKey designpart then
            memo[designpart]
        else
            if designpart = "" then true
            else
                let minlength = min maxSize designpart.Length
                let mutable found = false
                for l in 0..minlength do
                    let subpart = designpart.Substring(0, l)
                    if Set.contains subpart patterns then
                        if canBeDone patterns (designpart.Substring(subpart.Length)) then
                            found <- true

                memo.Add(designpart, found)
                found

    canBeDone patterns design

let execute() =
    let path = "day19/day19_input.txt"
    let content = LocalHelper.GetContentFromFile path
    let (patterns, designs) = parseContent content
    let maxPatternSize = ((patterns |> Array.sortByDescending _.Length)[0]).Length
    let memo = Dictionary<string, bool>()
    let mutable found = 0
    designs
    |> Array.sumBy(fun d ->
        if checkIfValid d (patterns |> Set.ofArray) maxPatternSize memo then 1 else 0
    )