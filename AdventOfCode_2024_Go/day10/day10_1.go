package day10

import (
	"fmt"

	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

type Coord struct {
	Row int
	Col int
}

type Node struct {
	Name      int
	Position  Coord
	Neighbors []*Node
}

func FindPath(initnodes []Node) int {
	found := make([]Coord, 0) //make(map[Coord]bool)

	var FindPathRecursive func(node Node)
	FindPathRecursive = func(node Node) {
		fmt.Printf("Node: %d\n", node.Name)
		if node.Name == 9 {
			found = append(found, node.Position)
		} else {
			for _, neighbor := range node.Neighbors {
				fmt.Printf("Neighbor: %d\n", neighbor.Name)
				if neighbor.Name-node.Name == 1 {
					FindPathRecursive(*neighbor)
				}
			}
		}
	}

	for _, node := range initnodes {
		FindPathRecursive(node)
	}

	return len(found)
}

func Executepart1() int {
	var result int = 0

	var fileName string = "./day10/day10.txt"

	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		result = len(fileContent)
		nodes := make(map[Coord]Node)
		initnodes := make([]Node, 0)
		for i, line := range fileContent {
			for j, char := range line {
				coord := Coord{i, j}
				nodes[coord] = Node{int(char - '0'), coord, make([]*Node, 0)}
			}
		}
		for _, node := range nodes {
			if node.Name == 0 {
				initnodes = append(initnodes, node)
			}
			for _, i := range []int{1, 0, -1} {
				for _, j := range []int{1, 0, -1} {
					if i == 0 && j == 0 {
						continue
					}
					neighbor := Coord{node.Position.Row + i, node.Position.Col + j}
					if neighborNode, ok := nodes[neighbor]; ok {
						node.Neighbors = append(node.Neighbors, &neighborNode)
					}
				}
			}
		}
		result = FindPath(initnodes)
	}

	return result
}
