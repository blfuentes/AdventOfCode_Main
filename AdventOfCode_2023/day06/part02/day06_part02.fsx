#load @"../../Modules/Utilities.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_2023.Modules

//let path = "day06/test_input_01.txt"
let path = "day06/day06_input.txt"

let bigint = System.Numerics.BigInteger.Parse

type RaceTime = {
    time: bigint
    distance: bigint
}

let holdMakesYouWin (race: RaceTime) (pushTime: bigint) =
    let remaining = race.time - pushTime
    let distance = remaining * pushTime
    race.distance < distance

let getWinners (race: RaceTime) (timers: bigint array) =
    timers |> Array.filter (holdMakesYouWin race)

let execute =
    let raceTimes = 
        let lines = Utilities.GetLinesFromFile path
        let timeLine = lines.[0]
        let distanceTime = lines.[1]
        let times = Regex.Matches(timeLine, @"\d+") |> List.ofSeq |> List.map (fun m -> m.Value) |> String.concat ""
        let distances = Regex.Matches(distanceTime, @"\d+") |> List.ofSeq |> List.map (fun m -> m.Value) |> String.concat ""
        let races = 
            seq {
                yield { time = bigint (times); distance = bigint(distances) }
            } |> List.ofSeq
        races
    let results = raceTimes |> List.map(fun r -> getWinners r [|1I..r.time - 1I|])
    results |> List.map _.Length |> List.reduce (*)