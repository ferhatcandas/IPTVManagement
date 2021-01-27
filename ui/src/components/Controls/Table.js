import { TableContainer as TableWrapper, Table, TableHead, TableRow, TableBody, TableCell, Paper, TablePagination, TextField } from "@material-ui/core";
import { Search as SearchIcon } from "@material-ui/icons";
import { useState } from "react";
import createElement from "./Element";
const TableContainer = (props) => {
    const { data, headers } = props
    const [page, setPage] = useState(0);
    const [pageRows, setRows] = useState(10);
    const [filterText, setFilterText] = useState("");

    const handleChangePage = (event, newPage) => {
        setPage(newPage);
    };
    const handleChangeRowsPerPage = (event) => {
        setRows(+ event.target.value);
        setPage(0);
    };
    const filterHandle = (event) => {
        if (event.target.value) {
            setFilterText(event.target.value.toLowerCase());
            filterResultSet(data);
        }
        else {
            //
        }
        event.preventDefault();
    }
    const filterResultSet = (data) => {
        if (filterText.length > 0) {
            // let newFiltredChannels = data.filter(x =>
            //     x?.name.toLowerCase().includes(filterText) ||
            //     x?.category.toLowerCase().includes(filterText) ||
            //     x?.language.toLowerCase().includes(filterText) ||
            //     x?.country.toLowerCase().includes(filterText) ||
            //     x?.statusCode?.toLowerCase().includes(filterText)
            // )
            //contains
            // setFilterData(data)
        }
        else {
            // setFilterData(data)
        }
    }
    const createTableCell = (header, element, index) => {
        header.value = element[header.dataName]
        header.text = element[header.dataName]
        if (header.type === "buttonGroup") {
            header.controls.forEach((x, index) => {
                if (x.condition) {
                    x[x.condition] = element[x.condition]
                }
                if (x["onClick"] != null) {
                    let callback = x["onClick"]
                    x["onClick"] = function () {
                        callback(element)
                    }
                }
            })
        }
        if (header["onClick"] != null) {
            let callback = header["onClick"]
            header["onClick"] = function () {
                callback(element)
            }

        }
        return (
            <TableCell key={index} align={header.align}>
                {createElement(header)}
            </TableCell>
        )
    }
    const searchPanel = () => {
        if (headers) {
            let exist = false;
            headers.forEach((element, index) => {
                if (element.search) {
                    exist = true
                    return
                }
            })
            if (exist)
                return (
                    <Table>
                        <TableHead>
                            <TableRow>
                                <TableCell align="right">
                                    <TextField placeholder="Search.." onChange={(e) => filterHandle(e)} InputProps={{ startAdornment: (<SearchIcon />) }} />
                                </TableCell>
                            </TableRow>
                        </TableHead>
                    </Table>
                )
        }
        return null
    }
    return (
        <>
            <TablePagination
                rowsPerPageOptions={[10, 25, 100]}
                component="div"
                count={data.length}
                rowsPerPage={pageRows}
                page={page}
                onChangePage={handleChangePage}
                onChangeRowsPerPage={handleChangeRowsPerPage}
            />
            <TableWrapper component={Paper}>
                {searchPanel()}
                <Table>
                    <TableHead>
                        <TableRow>
                            {headers.map((element, index) => {
                                return <TableCell key={index} align={element.align}>{element.name}</TableCell>
                            })}
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {data.slice(page * pageRows, page * pageRows + pageRows).map((element, index) => (
                            <TableRow key={index}>
                                {headers.map((headerElement, headerIndex) => {
                                    return createTableCell(headerElement, element, index + "_" + headerIndex)
                                })}
                                {/* <TableCell align="center">
                                    {(element.isEditable ?
                                        <>
                                            <IconButton onClick={() => handleOpenEditModal(element.name, element.id)}>
                                                <EditIcon />
                                            </IconButton>
                                            <IconButton onClick={() => handleDeleteAlertPopup(element.name, element.id)}>
                                                <DeleteIcon />
                                            </IconButton>
                                        </>
                                        : null
                                    )}
                                    <IconButton onClick={() => handleDuplicatePopup(element)}>
                                        <CopyIcon />
                                    </IconButton>
                                </TableCell> */}
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableWrapper>
        </>
    )
}
export default TableContainer