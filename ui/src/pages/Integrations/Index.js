import React, { useState } from 'react'
import { withRouter } from 'react-router-dom'
import TableContainer from "../../components/Controls/Table";
import { ChannelService } from "../../services/ChannelService";
import Popup from "../../components/Modal/Popup";
import YesNoPopup from "../../components/Modal/YesNoPopup";
import ChannelForm from "./ChannelForm";
import ReactHlsPlayer from "react-hls-player";

function Integrations() {
    const [data, setData] = useState([]);
    const [openPopup, setOpenPopup] = useState(false)
    const [popupTitle, setPopupTitle] = useState("")
    const [PopupContent, setPopupContent] = useState(<><h1>test</h1></>);
    const loadData = (refresh) => {
        if (data.length === 0 || refresh) {
            ChannelService.getAll((data) => {
                setData(data)
            })
        }

    };
    const handleOpenEditModal = (element) => {
        setPopupTitle(element.name);
        ChannelService.get(element.id, (data) => {
            setPopupContent(<ChannelForm close={setOpenPopup} data={data} callback={AddOrUpdateChannel} />)
            setOpenPopup(true);
        })
    }
    const handleDuplicatePopup = (element) => {
        let copyOfChannel = Object.assign({}, element);
        copyOfChannel.id = null;
        setPopupTitle("Copy channel")
        copyOfChannel.name = "Copy of " + copyOfChannel.name
        setPopupContent(<YesNoPopup buttonText="Yes" content="Kanalı kopyalamak istediğinize emin misiniz ?" callback={(response) => response ? AddOrUpdateChannel(copyOfChannel) : null} />)
        setOpenPopup(true);
    }
    const handleDeleteAlertPopup = (element) => {
        setPopupTitle(element.name);
        setPopupContent(<YesNoPopup buttonText="Delete" content="Kanalı silmek istediğinize emin misiniz ?" callback={(response) => response ? DeleteChannel(element.id) : null} />)
        setOpenPopup(true);
    }
    const AddOrUpdateChannel = (channel) => {
        if (channel.id) {
            ChannelService.put(channel, () => {
                setOpenPopup(false);
                loadData(true);
            });
        }
        else {
            ChannelService.post(channel, () => {
                setOpenPopup(false);
                loadData(true);
            });
        }

    }
    const DeleteChannel = (channelId) => {
        ChannelService.delete(channelId, () => {
            setOpenPopup(false);
            loadData(true);
        })
    }
    const streamPopup = (element) => {
        setPopupTitle(element.name);
        setPopupContent(
            <ReactHlsPlayer url={element.stream} autoPlay={true} controls={true} width="100%" height="100%" hlsConfig={{
                xhrSetup: (e) => {
                    e.onerror = (err) => {
                        setPopupTitle(element.name + " [Video Not Found]")
                    }
                }
            }} />
        )
        setOpenPopup(true);
    }
    const headers = [
        {
            "dataName": "logo",
            "name": "Logo",
            "search": false,
            "align": "center",
            "type": "image",
            "width": 45,
            "height": 45,
            onClick: streamPopup
        },
        {
            "dataName": "name",
            "name": "Channel Name",
            "search": true,
            "align": "center",
            "type": "text"
        },
        {
            "dataName": "category",
            "name": "Category",
            "search": true,
            "align": "center",
            "type": "text"
        },
        {
            "dataName": "language",
            "name": "Language",
            "search": true,
            "align": "center",
            "type": "text"
        },
        {
            "dataName": "country",
            "name": "Country",
            "search": true,
            "align": "center",
            "type": "text"
        },
        {
            "dataName": "statusCode",
            "name": "Status",
            "search": true,
            "align": "center",
            "type": "text"
        },
        {
            "dataName": "id",
            "name": "Actions",
            "align": "center",
            "type": "buttonGroup",
            "controls": [
                {
                    "type": "editButton",
                    "condition": "isEditable",
                    "onClick": handleOpenEditModal
                },
                {
                    "type": "deleteButton",
                    "condition": "isEditable",
                    "onClick": handleDeleteAlertPopup
                },
                {
                    "type": "copyButton",
                    "onClick": handleDuplicatePopup
                }
            ]
        }
    ]
    loadData();
    return (
        <>
            <TableContainer data={data} headers={headers} />
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
export default withRouter(Integrations)