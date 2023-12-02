﻿#load @"../../Modules/Utilities.fs"

open System
open System.Collections.Generic

open AdventOfCode_2023.Modules

let path = "day02/test_input_01.txt"
//let path = "day02/day02_input.txt"

let lines = Utilities.GetLinesFromFile path
//rule: 12 red cubes, 13 green cubes, and 14 blue cubes
type GameCube = {
    numberOfCubes: int;
    color: string;
}

type GameSet = {
    idGame: int;
    cubes: GameCube list list
}

let gameRules = [
    { numberOfCubes = 12; color = "red" };
    { numberOfCubes = 13; color = "green" };
    { numberOfCubes = 14; color = "blue" }
]

let buildGame (line: string) =
    let gameParts = line.Split(':')
    let cubeSets = 
        gameParts.[1].Split(';') |> Array.map(fun cubeSet -> 
            cubeSet.Split(',') |> Array.map(fun cube -> 
                let cubeParts = cube.Split(' ')
                { numberOfCubes = int cubeParts.[1]; color = cubeParts.[2] }
            ) |> List.ofArray
        ) |> List.ofArray
    { idGame = int (gameParts.[0].Split(' ').[1]); cubes = cubeSets }

let findMininumNumOfCubes (gameSet: GameSet) =
    let allCubes = gameSet.cubes |> List.concat
    let minCubesByColor = 
        allCubes |> List.groupBy(fun cube -> cube.color) 
            |> List.map (fun (co, cu) -> { numberOfCubes = (cu |> List.maxBy _.numberOfCubes).numberOfCubes ; color = co })
    //minCubesByColor
    minCubesByColor |> List.map _.numberOfCubes |> List.reduce (*)

        

let processGame (lines: string array) =
    let minimunByGame = lines |> Array.map buildGame |> Array.map(fun g -> findMininumNumOfCubes g)
    minimunByGame |> Array.sum

let result = processGame lines