package day16

import (
	"container/heap"
	"math"

	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

type PositionDir struct {
	Pos Position
	Dir int
}

type PositionDirDistance struct {
	PDir     PositionDir
	Distance int
}

func buildGraph(themap [][]string) [][][]Cell {
	maxrows, maxcols := len(themap), len(themap[0])
	graph := make([][][]Cell, 0)
	for row := 0; row < maxrows; row++ {
		rowdimension := make([][]Cell, 0)
		for col := 0; col < maxcols; col++ {
			dirdimension := make([]Cell, 0)
			for dir := 0; dir < 4; dir++ {
				pos := Cell{false, math.MaxInt32}
				dirdimension = append(dirdimension, pos)
			}
			rowdimension = append(rowdimension, dirdimension)
		}
		graph = append(graph, rowdimension)
	}

	return graph
}

var isValidCoord = func(position Position, maxrows, maxcols int, themap [][]string) bool {
	return position.Row >= 0 && position.Col >= 0 &&
		position.Row < maxrows && position.Col < maxcols &&
		themap[position.Row][position.Col] != "#"
}
var alreadyVisited = func(pos Position, dir int, graph [][][]Cell) bool {
	return graph[pos.Row][pos.Col][dir].Visited
}
var setDistance = func(pos Position, dir, distance int, graph *[][][]Cell) {
	(*graph)[pos.Row][pos.Col][dir].Distance = distance
}
var setVisited = func(pos Position, dir int, graph *[][][]Cell) {
	(*graph)[pos.Row][pos.Col][dir].Visited = true
}

func consumepath(queue *utilities.PriorityQueue[PositionDirDistance],
	graph *[][][]Cell, maxrows, maxcols int, themap [][]string, endnode Position) int {
	if queue.Len() == 0 {
		lowest := math.MaxInt32
		for d := 0; d < 4; d++ {
			if (*graph)[endnode.Row][endnode.Col][d].Distance <= lowest {
				lowest = (*graph)[endnode.Row][endnode.Col][d].Distance
			}
		}
		return lowest
	}

	current := heap.Pop(queue).(PositionDirDistance)
	if alreadyVisited(current.PDir.Pos, current.PDir.Dir, *graph) {
		return consumepath(queue, graph, maxrows, maxcols, themap, endnode)
	}

	setVisited(current.PDir.Pos, current.PDir.Dir, graph)
	neighbours := Neighbours(current.PDir.Pos, current.PDir.Dir)
	for _, n := range neighbours {
		if isValidCoord(n.PDir.Pos, maxrows, maxcols, themap) &&
			!(alreadyVisited(n.PDir.Pos, n.PDir.Dir, *graph)) {
			n.Distance = current.Distance + n.Distance

			if n.Distance <= (*graph)[n.PDir.Pos.Row][n.PDir.Pos.Col][n.PDir.Dir].Distance {
				setDistance(n.PDir.Pos, n.PDir.Dir, n.Distance, graph)
				heap.Push(queue, n)
			}
		}
	}

	return consumepath(queue, graph, maxrows, maxcols, themap, endnode)
}

func djikstraExplore(themap [][]string, startnode, endnode Position) int {
	maxrows, maxcols := len(themap), len(themap[0])
	graph := buildGraph(themap)
	queue := make(utilities.PriorityQueue[PositionDirDistance], 0)
	heap.Init(&queue)

	setDistance(startnode, 0, 0, &graph)
	sn := PositionDirDistance{PositionDir{Position{startnode.Row, startnode.Col}, 0}, 0}
	heap.Push(&queue,
		&utilities.Item[PositionDirDistance]{})

	return consumepath(&queue, &graph, maxrows, maxcols, themap, endnode)
}

func Executepart1() int {
	var result int = 0

	// var fileName string = "./day16/day16.txt"
	var fileName string = "./day16/test_input_16.txt"

	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		themap, startnode, endnode := parseContent(fileContent)
		result = djikstraExplore(themap, startnode, endnode)

	}

	return result
}
