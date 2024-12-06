module day06_part02

open AdventOfCode_2024.Modules
open System.Collections.Generic

type Direction =
    | UP
    | DOWN
    | RIGHT
    | LEFT

type MapDefinition = {
    Definition : char [,]
    PatrolPos: int array;
    PatrolDirection : Direction
    Blockers: int array list
}

let getDir(dir: Direction) =
    match dir with
    | UP -> [| -1; 0 |]
    | DOWN -> [| 1; 0 |]
    | RIGHT -> [| 0; 1 |]
    | LEFT -> [| 0; -1 |]

let turnRight(dir: Direction) =
    match dir with
    | UP -> RIGHT
    | DOWN -> LEFT
    | RIGHT -> DOWN
    | LEFT -> UP

let parseContent(lines: string array) =
    let map = Array2D.create lines.Length lines[0].Length '.'
    let patrolPos = [|0; 0|]

    let blockers =
        seq {
            for rIdx in 0..lines.Length-1 do
                for cIdx in 0..lines[rIdx].Length-1 do
                    let value = lines[rIdx][cIdx]
                    if value = '^' then
                        patrolPos[0] <- rIdx
                        patrolPos[1] <- cIdx
                    if value = '#' then
                        yield [|rIdx; cIdx|]
                    map[rIdx, cIdx] <- lines[rIdx][cIdx]
        }
    { Definition = map; PatrolPos = patrolPos; PatrolDirection = UP; Blockers = blockers |> List.ofSeq }

let printMap (map: char[,]) =
    for row in [0..map.GetUpperBound(0)] do
        for column in [0..map.GetUpperBound(1) ] do
            printf "%c" map[row, column]
        printfn ""

let outOfBoundaries(row: int) (col: int) (maxRows: int) (maxCols: int) =
    row >= 0 && row < maxRows && col >= 0 && col < maxCols

let patrol(patrol: MapDefinition) (newWall: int array) =
    let mutable outOfRange = false
    let mutable currentDirection = patrol.PatrolDirection
    let pos = [|patrol.PatrolPos[0]; patrol.PatrolPos[1]|]
    let maxRows = patrol.Definition.GetLength(0)
    let maxCols = patrol.Definition.GetLength(1)
    let visitedMap = Array2D.create maxRows maxCols '.'
    visitedMap[pos[0], pos[1]] <- 'X'
    let mutable counter = 1
    visitedMap[newWall[0], newWall[1]] <- '0'
    let memoTable = new Dictionary<int*int* Direction, bool>()

    while not outOfRange do
        if memoTable.ContainsKey((pos[0], pos[1], currentDirection)) then
            counter <- -1
            outOfRange <- true
        else
            memoTable.Add((pos[0], pos[1], currentDirection), true)

            let currentDir = getDir currentDirection
            pos[0] <- pos[0] + currentDir[0]
            pos[1] <- pos[1] + currentDir[1]
            if not (outOfBoundaries pos[0] pos[1] maxRows maxCols) then
                outOfRange <- true
            else
                let mapvalue = patrol.Definition[pos[0], pos[1]]
                if mapvalue <> '#' && not (pos[0] = newWall[0] && pos[1] = newWall[1]) then
                    if visitedMap[pos[0], pos[1]] <> 'X' then
                        visitedMap[pos[0], pos[1]] <- 'X'
                        counter <- counter + 1
                else
                    visitedMap[pos[0], pos[1]] <- mapvalue
                    visitedMap[newWall[0], newWall[1]] <- '0'
                    pos[0] <- pos[0] - currentDir[0]
                    pos[1] <- pos[1] - currentDir[1]
                    currentDirection <- turnRight currentDirection
    (visitedMap, counter)

let checkWalls(map: MapDefinition) =
    let (visited, counted) = patrol map map.PatrolPos
    let maxRows = visited.GetLength(0)
    let maxCols = visited.GetLength(1)
    let mutable wallsfound = 0
    for row in 0..maxRows-1 do
        for col in 0..maxCols-1 do
            if visited[row, col] = 'X' then
                let (visited, counted) = patrol map [|row; col|]
                if counted = -1 then
                    wallsfound <- wallsfound + 1

    wallsfound
                

let execute =
    let path = "day06/day06_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let mapDefinition = parseContent content
    let counted = checkWalls mapDefinition 
    counted