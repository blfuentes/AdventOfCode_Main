module day16_part01

open AdventOfCode_2023.Modules

type Direction = LEFT | RIGHT | UP | DOWN | NONE

type CellType = EMPTY | LMIRROR | RMIRROR | VSPLITTER | HSPLIITER

type Beam = { id: int; x: int; y: int; direction: Direction; visited: Beam list; loop: bool }

let switchOrientation (d: Direction) (c: CellType) =
    match d, c with
    // right beam
    | RIGHT, EMPTY -> [ RIGHT ]
    | RIGHT, LMIRROR -> [ DOWN ]
    | RIGHT, RMIRROR -> [ UP ]
    | RIGHT, VSPLITTER -> [ UP; DOWN ]
    | RIGHT, HSPLIITER -> [ RIGHT ]

    // left beam
    | LEFT, EMPTY -> [ LEFT ]
    | LEFT, LMIRROR -> [ UP ]
    | LEFT, RMIRROR -> [ DOWN ]
    | LEFT, VSPLITTER -> [ UP; DOWN ]
    | LEFT, HSPLIITER -> [ LEFT ]

    // up beam
    | UP, EMPTY -> [ UP ]
    | UP, LMIRROR -> [ LEFT ]
    | UP, RMIRROR -> [ RIGHT ]
    | UP, VSPLITTER -> [ UP ]
    | UP, HSPLIITER -> [ LEFT; RIGHT ]

    // down beam
    | DOWN, EMPTY -> [ DOWN ]
    | DOWN, RMIRROR -> [ LEFT ]
    | DOWN, LMIRROR -> [ RIGHT ]
    | DOWN, VSPLITTER -> [ DOWN ]
    | DOWN, HSPLIITER -> [ LEFT; RIGHT ]

    | _ -> failwith "Unknown direction"

let parseInput (input: string list) =
    let grid = Array2D.create input.Length input.[0].Length EMPTY
    for i in 0 .. input.Length - 1 do
        for j in 0 .. input.[i].Length - 1 do
            match input.[i].[j] with
            | '.' -> grid.[i, j] <- EMPTY
            | '/' -> grid.[i, j] <- RMIRROR
            | '\\' -> grid.[i, j] <- LMIRROR
            | '|' -> grid.[i, j] <- VSPLITTER
            | '-' -> grid.[i, j] <- HSPLIITER
            | _ -> failwith "Unknown cell type"
    grid

let printGrid ( grid: CellType [,] ) =
    for i in 0 .. grid.GetLength(0) - 1 do
        for j in 0 .. grid.GetLength(1) - 1 do
            match grid.[i, j] with
            | EMPTY -> printf "."
            | LMIRROR -> printf "\\"
            | RMIRROR -> printf "/"
            | VSPLITTER -> printf "|"
            | HSPLIITER -> printf "-"
        printfn ""

let getPos (pos: int[]) (d: Direction) =
    match d with
    | LEFT -> (d, [| pos.[0]; pos.[1] - 1 |])
    | RIGHT -> (d, [| pos.[0]; pos.[1] + 1 |])
    | UP -> (d, [| pos.[0] - 1; pos.[1] |])
    | DOWN -> (d, [| pos.[0] + 1; pos.[1] |])
    | NONE -> (d, pos)

let rec moveBeam (grid: CellType [,]) (beams: Beam list) (visited: int[] list) =
    match beams |> List.filter (fun b -> not b.loop) with
    | beam :: rest ->            
        let orientations = switchOrientation beam.direction grid.[beam.x, beam.y]
        let newBeams =
            seq {
                for idx in 0..orientations.Length - 1 do
                    let (d, pos) = getPos [| beam.x; beam.y |] orientations.[idx]
                    if pos.[0] >= 0 && pos.[0] < grid.GetLength(0) && pos.[1] >= 0 && pos.[1] < grid.GetLength(1) then
                        let newId = beam.id + if orientations.Length = 1 then 0 else idx; 
                        let isloop = beam.visited |> List.exists (fun v -> v.x = pos.[0] && v.y = pos.[1] && v.direction = d)
                        let newVisited = beam.visited @ [ { id = newId; x = pos.[0]; y = pos.[1]; direction = d; visited = []; loop = false } ]
                        let newBeam = { 
                            id = newId
                            x = pos.[0]; 
                            y = pos.[1]; 
                            direction = d; 
                            visited = newVisited; 
                            loop = isloop
                        }
                        yield (newBeam, pos)
            } |> List.ofSeq
        let beams' = newBeams |> List.map fst
        let positions = newBeams |> List.map snd
        visited @ positions @ moveBeam grid (beams' @ rest) visited
    | [] -> visited

let execute =
    //let path = "day16/test_input_01.txt"
    let path = "day16/day16_input.txt"
    let input = LocalHelper.ReadLines path |> List.ofSeq
    let grid = parseInput input
    printGrid grid
    let initBeam = { id = 0; x = 0; y = 0; direction = RIGHT; visited = []; loop = false }
    let visited = moveBeam grid [ initBeam ] [[| 0; 0 |]]
    visited |> List.distinct |> List.length