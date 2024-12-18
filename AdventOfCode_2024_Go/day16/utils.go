package day16

import "strings"

type Position struct {
	Row int
	Col int
}

type Cell struct {
	Visited  bool
	Distance int
}

func parseContent(lines []string) ([][]string, Position, Position) {
	maxrows, maxcols := len(lines), len(lines[0])
	themap := make([][]string, 0)
	start, end := Position{-1, -1}, Position{-1, -1}

	for row := 0; row < maxrows; row++ {
		line := strings.Split(lines[row], "")
		mapline := make([]string, 0)
		for col := 0; col < maxcols; col++ {
			value := line[col]
			mapline = append(mapline, value)
			if value == "S" {
				start.Row, start.Col = row, col
			} else if value == "E" {
				end.Row, end.Col = row, col
			}
		}
		themap = append(themap, mapline)
	}

	return themap, start, end
}

func Neighbours(position Position, dir int) [3]PositionDirDistance {
	clockwisecell := PositionDirDistance{PositionDir{position, -1}, 1000}
	counterwisecell := PositionDirDistance{PositionDir{position, -1}, 1000}
	neigbour := PositionDirDistance{PositionDir{Position{-1, -1}, dir}, 1}
	switch dir {
	case 0: // east
		neigbour.PDir.Pos.Row = position.Row
		neigbour.PDir.Pos.Col = position.Col + 1
	case 1: // south
		neigbour.PDir.Pos.Row = position.Row + 1
		neigbour.PDir.Pos.Col = position.Col
	case 2: // west
		neigbour.PDir.Pos.Row = position.Row
		neigbour.PDir.Pos.Col = position.Col - 1
	case 3: // north
		neigbour.PDir.Pos.Row = position.Row - 1
		neigbour.PDir.Pos.Col = position.Col
	}
	clockwisecell.PDir.Dir = (dir + 1) % 4
	counterwisecell.PDir.Dir = (dir + 3) % 4

	return [3]PositionDirDistance{neigbour, clockwisecell, counterwisecell}
}
