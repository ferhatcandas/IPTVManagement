import React, { useState } from 'react'
import { withRouter } from 'react-router-dom'
import { TableContainer, Table, TableHead, TableRow, TableBody, TableCell, Paper, TablePagination, IconButton, TextField } from "@material-ui/core";
import EditIcon from "@material-ui/icons/Edit";
import SearchIcon from "@material-ui/icons/Search";
import DeleteIcon from "@material-ui/icons/Delete";
import { ChannelService } from "../../services/ChannelService";
import Popup from "../../components/Modal/Popup";
import YesNoPopup from "../../components/Modal/YesNoPopup";
import ChannelForm from "./ChannelForm";
import ReactHlsPlayer from "react-hls-player";




function TvChannels() {
    const [channels, setChannels] = useState([]);
    const [filtredChannels, setFiltredChannels] = useState([]);
    const [page, setPage] = useState(0);
    const [pageRows, setRows] = useState(10);
    const [openPopup, setOpenPopup] = useState(false)
    const [popupTitle, setPopupTitle] = useState("")
    const [PopupContent, setPopupContent] = useState(<><h1>test</h1></>);

    const loadChannels = (refresh) => {
        if (filtredChannels.length === 0 || refresh) {
            ChannelService.getAll((data) => {
                setChannels(data)
                setFiltredChannels(data)
            })
        }

    };
    const handleChangePage = (event, newPage) => {
        setPage(newPage);
    };
    const handleChangeRowsPerPage = (event) => {
        setRows(+ event.target.value);
        setPage(0);
    };

    const handleOpenEditModal = (title, id) => {
        setPopupTitle(title);
        ChannelService.get(id, (data) => {
            setPopupContent(<ChannelForm data={data} callback={AddOrUpdateChannel} />)
            setOpenPopup(true);
        })
    }
    const handleDeleteAlertPopup = (title, id) => {
        setPopupTitle(title);
        setPopupContent(<YesNoPopup content="Kanalı silmek istediğiniz emin misiniz ?" callback={(response) => response ? DeleteChannel(id) : null} />)
        setOpenPopup(true);
    }
    const AddOrUpdateChannel = (channel) => {

        if (channel.id) {
            ChannelService.put(channel, () => {
                setOpenPopup(false);
                loadChannels(true);
            });
        }
        else {
            ChannelService.post(channel, () => {
                setOpenPopup(false);
                loadChannels(true);
            });
        }

    }
    const DeleteChannel = (channelId) => {
        ChannelService.delete(channelId, () => {
            setOpenPopup(false);
            loadChannels(true);
        })
    }
    const streamPopup = (title, link) => {
        setPopupTitle(title);
        setPopupContent(
            <ReactHlsPlayer url={link} autoPlay={true} controls={true} width="100%" height="100%" hlsConfig={{
                xhrSetup: (e) => {
                    console.log(e)
                    e.onerror = (err) => {
                        setPopupTitle(title + " [Video Not Found]")
                    }
                }
            }} />
        )
        setOpenPopup(true);
    }
    const filterResultSet = (value) => {
        console.log(value)
        value = value.toLowerCase();
        if (value) {
            let newFiltredChannels = channels.filter(x =>
                x?.name.toLowerCase().includes(value) ||
                x?.category.toLowerCase().includes(value) ||
                x?.language.toLowerCase().includes(value) ||
                x?.country.toLowerCase().includes(value)
            )
            setFiltredChannels(newFiltredChannels)
        }

    }
    loadChannels();
    return (
        <>
            <TablePagination
                rowsPerPageOptions={[10, 25, 100]}
                component="div"
                count={filtredChannels.length}
                rowsPerPage={pageRows}
                page={page}
                onChangePage={handleChangePage}
                onChangeRowsPerPage={handleChangeRowsPerPage}
            />
            <TableContainer component={Paper}>
                <Table>
                    <TableHead>
                        <TableRow>
                            <TableCell align="right">
                                <TextField placeholder="Search.." onChange={(e) => filterResultSet(e.target.value)} InputProps={{ startAdornment: (<SearchIcon />) }} />
                            </TableCell>
                        </TableRow>
                    </TableHead>
                </Table>
                <Table>
                    <TableHead>
                        <TableRow>
                            <TableCell align="center">Logo</TableCell>
                            <TableCell align="center">Name</TableCell>
                            <TableCell align="center">Category</TableCell>
                            <TableCell align="center">Country</TableCell>
                            <TableCell align="center">Language</TableCell>
                            <TableCell align="center">Actions</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {filtredChannels.slice(page * pageRows, page * pageRows + pageRows).map((element, index) => (
                            <TableRow key={index}>
                                <TableCell align="center"><img style={{ cursor: "pointer" }} onClick={() => streamPopup(element.name, element.stream)} alt={element.name} src={element.logo} width={40} height={40} /></TableCell>
                                <TableCell align="center">{element.name}</TableCell>
                                <TableCell align="center">{element.category}</TableCell>
                                <TableCell align="center">{element.country}</TableCell>
                                <TableCell align="center">{element.language}</TableCell>
                                <TableCell align="center">
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
                                </TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
            <Popup
                openPopup={openPopup}
                setOpenPopup={setOpenPopup}
                title={popupTitle}
            >
                {PopupContent}
            </Popup>
        </>
    )
}
export default withRouter(TvChannels)


