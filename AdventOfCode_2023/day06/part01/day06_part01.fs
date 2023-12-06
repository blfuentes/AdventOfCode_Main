module day06_part01

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_2023.Modules

let path = "day06/day06_input.txt"

type RaceTime = {
    time: int
    distance: int
}

let holdMakesYouWin (race: RaceTime) (pushTime: int) =
    let remaining = race.time - pushTime
    let distance = remaining * pushTime
    race.distance < distance

let getWinners (race: RaceTime) (timers: int array) =
    timers |> Array.filter (holdMakesYouWin race)

let execute =
    let raceTimes = 
        let lines = Utilities.GetLinesFromFile path
        let timeLine = lines.[0]
        let distanceTime = lines.[1]
        let times = Regex.Matches(timeLine, @"\d+")
        let distances = Regex.Matches(distanceTime, @"\d+")
        let races = 
            seq {
                for idx in 0..times.Count-1 do
                    yield { time = Int32.Parse(times.[idx].Value); distance = Int32.Parse(distances.[idx].Value) }
            } |> List.ofSeq
        races
    let results = raceTimes |> List.map(fun r -> getWinners r [|1..r.time - 1|])
    results |> List.map _.Length |> List.reduce (*)