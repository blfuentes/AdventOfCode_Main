module day17_part01

open System
open AdventOfCode_2023.Modules

type Node = { Row: int; Col: int; Value: int; }

type Edge = { Start: Node; End: Node; Weight: int }

let dijkstraWithConstraints startNode endNode graph =
    let distance = Map.empty |> Map.add startNode 0
    let directionCount = Map.empty |> Map.add startNode 0

    let rec relaxEdges (currentNode: Node) (currentDistance: int) (currentDirectionCount: int) =
        graph
        |> Seq.filter (fun edge -> edge.Start = currentNode || edge.End = currentNode)
        |> Seq.filter (fun edge ->
            let neighbor = if edge.Start = currentNode then edge.End else edge.Start
            let neighborDistance = Map.tryFind neighbor distance
            match neighborDistance with
            | Some d -> currentDistance + edge.Weight < d
            | None -> true
        )
        |> Seq.iter (fun edge ->
            let neighbor = if edge.Start = currentNode then edge.End else edge.Start
            let neighborDistance = currentDistance + edge.Weight
            let updatedDirectionCount =
                if currentDirectionCount + 1 > 3 then 0
                else currentDirectionCount + 1

            let isSmallerDistance = 
                match Map.tryFind neighbor distance with
                | Some d -> neighborDistance < d
                | None -> true

            if isSmallerDistance then
                relaxEdges neighbor neighborDistance updatedDirectionCount
        )

    let rec dijkstraLoop (visited: Set<Node>) =
        let unvisitedNodes =
            distance
            |> Map.filter (fun key _ -> not (Set.contains key visited))
            |> Map.fold (fun (minNode, minValue) key value ->
                if minValue = 0 || value < minValue then (key, value) else (minNode, minValue)
            ) ({ Row = -1; Col = -1; Value = 0 }, 0) // Initialize with a dummy node

        match unvisitedNodes with
        | (currentNode, currentDistance) ->
            relaxEdges currentNode currentDistance (Map.find currentNode directionCount)
            dijkstraLoop (Set.add currentNode visited)

    dijkstraLoop Set.empty

    // Retrieve the shortest path
    let rec reconstructPath (node: Node) (path: Node list) =
        if Map.containsKey node distance then          
            let previousNode = Map.tryFind node distance
            match previousNode with
            | Some(prev) ->
                let updatedPath = node :: path
                reconstructPath node updatedPath
            | None -> path
        else
            path

    match Map.tryFind endNode distance with
    | Some _ -> Some (reconstructPath endNode [endNode])
    | None -> None

let parseInput (input: string list) =
    let grid = Array2D.create (input.Length) (input.[0].Length) 0
    for i in 0..(input.Length-1) do
        for j in 0..(input.[0].Length-1) do
            grid.[i,j] <- (int)(input.[i].[j].ToString())
    grid

let generateGraph (grid: int[,]) =
    let nodes =
        seq {
            for i in 0..(grid.GetUpperBound(0)) do
            for j in 0..(grid.GetUpperBound(1)) do
                yield { Row = i; Col = j; Value = grid.[i,j] }     
        }
    let edges =
        seq {
            for i in 0..(grid.GetUpperBound(0)) do
                for j in 0..(grid.GetUpperBound(1)) do
                    let currentNode = { Row = i; Col = j; Value = grid.[i,j] }
                    let neighbors =
                        seq {
                            for k in -1..1 do
                                for l in -1..1 do
                                    if k <> 0 || l <> 0 then
                                        let neighborRow = i + k
                                        let neighborCol = j + l
                                        if neighborRow >= 0 && neighborRow <= grid.GetUpperBound(0) && 
                                            neighborCol >= 0 && neighborCol <= grid.GetUpperBound(1) then
                                            yield { Row = neighborRow; Col = neighborCol; Value = grid.[neighborRow, neighborCol] }
                        }
                    for neighbor in neighbors do
                        yield { Start = currentNode; End = neighbor; Weight = 1 }
        }
    nodes, edges


let execute =
    let path = "day17/test_input_01.txt"
    //let path = "day17/day17_input.txt"
    let lines = LocalHelper.ReadLines path |> List.ofSeq
    let matrix = parseInput lines

    let maxRow = matrix.GetUpperBound(0) + 1
    let maxCol = matrix.GetUpperBound(1) + 1
    let maxDistance = Int32.MaxValue
    let (nodes, edges) = generateGraph matrix
    let startingNode = nodes |> Seq.find (fun node -> node.Row = 0 && node.Col = 0)
    let endingNode = nodes |> Seq.find (fun node -> node.Row = maxRow - 1 && node.Col = maxCol - 1)
    let result = dijkstraWithConstraints startingNode endingNode edges
    match result with
    | Some path -> path |> List.sumBy _.Value
    | None -> 0
