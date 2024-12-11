module day11_part02

open AdventOfCode_2024.Modules
open System.Collections.Generic

let parseContent (lines: string) =
    let data = Dictionary<int64, int64>()
    lines.Split(" ")
    |> Array.iter (fun s ->
        let key = int64 s
        data[key] <-
            if data.ContainsKey(key) then
                data.[key] + 1L
            else
                1L
    )
    data

let runMurations (stonesdata: Dictionary<int64, int64>) (step: int) =
    let mutable currentStoneData = stonesdata
    [1..step]
    |> List.iter(fun _ ->
        let newStoneData = Dictionary<int64, int64>()
        for kvp in currentStoneData do
            let stonekey = kvp.Key
            let stoneData = kvp.Value
            let length = stonekey.ToString().Length
        
            match stonekey with
            | sk when sk = 0L ->
                newStoneData[1L] <- 
                    if newStoneData.ContainsKey(1) then 
                        newStoneData[1L] + stoneData 
                    else 
                        stoneData
            | sk when length % 2 = 0 ->
                let mid = sk.ToString().Length / 2
                let left, right = (
                    System.Int64.Parse(sk.ToString()[0..mid-1])
                    , System.Int64.Parse(sk.ToString()[mid..]))
                newStoneData[left] <- 
                    if newStoneData.ContainsKey(left) then
                        newStoneData[left] + stoneData
                    else
                        stoneData
                newStoneData[right] <- 
                    if newStoneData.ContainsKey(right) then
                        newStoneData[right] + stoneData
                    else
                        stoneData
            | _ ->
                let newStoneKey = stonekey * 2024L
                newStoneData[newStoneKey] <-
                    if newStoneData.ContainsKey(newStoneKey) then
                        newStoneData[newStoneKey] + stoneData
                    else
                        stoneData
        currentStoneData <- newStoneData
    )
        
    currentStoneData


let execute() =
    let path = "day11/day11_input.txt"
    let content = LocalHelper.GetContentFromFile path
    let stones = parseContent content
    let mutatedValues = runMurations stones 75

    mutatedValues.Values |> Seq.sum