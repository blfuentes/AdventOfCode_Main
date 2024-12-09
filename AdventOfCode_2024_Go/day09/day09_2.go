package day09

import (
	"fmt"
	"strings"

	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

type DiskFile struct {
	FileId int
	Space  int
}

func checksum2(files *[]DiskFile) int64 {
	return 0
}

func compactFiles2(files *[]DiskFile) []DiskFile {
	fileIdx := len((*files)) - 1
	gapIdx := 0
	firstgap := 0
	for fileIdx > firstgap {
		if (*files)[fileIdx].FileId != -1 {
			for gapIdx < fileIdx && ((*files)[gapIdx].FileId != -1 || (*files)[gapIdx].Space < (*files)[fileIdx].Space) {
				gapIdx++
			}
			if gapIdx < fileIdx {
				insertedFiles := DiskFile{(*files)[fileIdx].FileId, (*files)[fileIdx].Space}
				(*files)[gapIdx].Space -= (*files)[fileIdx].Space

				(*files) = append((*files)[:gapIdx], append([]DiskFile{insertedFiles}, (*files)[gapIdx:]...)...)

				(*files)[fileIdx+1].FileId = -1
				if (*files)[gapIdx].Space == 0 {

				}
				gapIdx = 0
			} else {
				fileIdx--
				gapIdx = 0
			}
		} else {
			fileIdx--
		}
		for firstgap := 0; firstgap < len(*files); firstgap++ {
			if (*files)[firstgap].Space > 0 {
				break
			}
		}
	}
	return (*files)[:gapIdx]
}

func Executepart2() int {
	var result int = 0

	// var fileName string = "./day09/day09.txt"
	var fileName string = "./day09/test_input_09.txt"

	if fileContent, err := utilities.ReadFileAsText(fileName); err == nil {
		fileIdx := 0
		var tmpIdx int
		disk := make([]DiskFile, 0)
		for cIdx, size := range strings.Split(fileContent, "") {
			// part := make([]int, size)
			if cIdx%2 == 0 {
				tmpIdx = fileIdx
				fileIdx++
			} else {
				tmpIdx = -1
			}
			diskfile := DiskFile{tmpIdx, utilities.StringToInt(size)}
			// sector := DiskSector{make([]DiskFile, 0)}
			// sector.Files = append(sector.Files, diskfile)
			disk = append(disk, diskfile)
		}
		compacteddisk := compactFiles2(&disk)
		// result = int(checksum(&compacteddisk))
		// fmt.Printf("%s\n", strings.Join(compacteddisk, ""))
		fmt.Printf("%d\n", len(compacteddisk))
	}

	return result
}
