package day09

import (
	"strings"

	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

type DiskFile struct {
	FileId int
	Space  int
}

func buildChecksum(files *[]DiskFile) int64 {
	values := make([]int, 0)
	for fIdx := 0; fIdx < len(*files); fIdx++ {
		size := (*files)[fIdx].Space
		if size > 0 {
			for sIdx := 0; sIdx < size; sIdx++ {
				values = append(values, (*files)[fIdx].FileId)
			}
		}
	}
	return checksum(&values)
}

func compactFiles2(files *[]DiskFile) []DiskFile {
	fileIdx := len((*files)) - 1
	gapIdx := 0
	for fileIdx > gapIdx {
		if (*files)[fileIdx].FileId != -1 {
			for gapIdx < fileIdx && ((*files)[gapIdx].FileId != -1 || (*files)[gapIdx].Space < (*files)[fileIdx].Space) {
				gapIdx++
			}
			if gapIdx < fileIdx {
				insertedFiles := DiskFile{(*files)[fileIdx].FileId, (*files)[fileIdx].Space}
				(*files)[gapIdx].Space -= (*files)[fileIdx].Space

				(*files) = utilities.InsertElementAt(gapIdx, insertedFiles, *files)

				(*files)[fileIdx+1].FileId = -1
				delPos := gapIdx + 1
				if (*files)[delPos].Space == 0 {
					(*files) = utilities.DeleteElementAt(delPos, *files)
				}
			} else {
				fileIdx--
			}
		} else {
			fileIdx--
		}
		for gapIdx = 0; gapIdx < len(*files); gapIdx++ {
			if (*files)[gapIdx].Space > 0 {
				break
			}
		}
	}
	return (*files)
}

func Executepart2() int {
	var result int = 0

	var fileName string = "./day09/day09.txt"

	if fileContent, err := utilities.ReadFileAsText(fileName); err == nil {
		fileIdx := 0
		var tmpIdx int
		disk := make([]DiskFile, 0)
		for cIdx, size := range strings.Split(fileContent, "") {
			if cIdx%2 == 0 {
				tmpIdx = fileIdx
				fileIdx++
			} else {
				tmpIdx = -1
			}
			diskfile := DiskFile{tmpIdx, utilities.StringToInt(size)}
			disk = append(disk, diskfile)
		}
		compacteddisk := compactFiles2(&disk)
		result = int(buildChecksum(&compacteddisk))
	}

	return result
}
