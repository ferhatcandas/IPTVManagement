import React, { useState } from 'react'
import Form from '../../components/Controls/Form';

// const useStyles = makeStyles((theme) => ({
//     fieldSpacing: {
//         paddingTop: theme.spacing(1)
//     }
// }))
export default function ChannelForm(props) {
    const { data, callback, close } = props
    const [channel, setChannel] = useState(data);
    const onChange = (event, name) => {
        let copyOfChannel = Object.assign({}, channel);
        copyOfChannel[name] = event.target.type === "checkbox" ? event.target.checked : event.target.value
        setChannel(copyOfChannel)
    }
    const channelForm = {
        "spacing": 2,
        "controls": [
            {
                "type": "container",
                "controls": [
                    {
                        "type": "image",
                        "xs": 1,
                        "value": channel?.logo,
                        "width": "45",
                        "height": "45"
                    },
                    {
                        "type": "textbox",
                        "xs": 11,
                        "fullWidth": true,
                        "onChange": onChange,
                        "name": "logo",
                        "placeHolder": "logo",
                        "value": channel?.logo
                    }
                ],
                "xs": 12
            },
            {
                "type": "textbox",
                "xs": 12,
                "fullWidth": true,
                "onChange": onChange,
                "name": "name",
                "placeHolder": "Channel Name",
                "value": channel?.name
            },
            {
                "type": "textbox",
                "xs": 12,
                "fullWidth": true,
                "onChange": onChange,
                "name": "category",
                "placeHolder": "News,Comic,Tv Shows",
                "value": channel?.category
            },
            {
                "type": "textbox",
                "xs": 12,
                "fullWidth": true,
                "onChange": onChange,
                "name": "country",
                "placeHolder": "TR,DZ,FR,EN...",
                "value": channel?.country
            },
            {
                "type": "textbox",
                "xs": 12,
                "fullWidth": true,
                "onChange": onChange,
                "name": "language",
                "placeHolder": "Turkish,Arabic,English...",
                "value": channel?.language
            },
            {
                "type": "textbox",
                "xs": 12,
                "fullWidth": true,
                "onChange": onChange,
                "name": "stream",
                "placeHolder": "....m3u8",
                "value": channel?.stream

            },
            {
                "type": "textbox",
                "xs": 12,
                "fullWidth": true,
                "onChange": onChange,
                "name": "tags",
                "placeHolder": "channel,channel2...",
                "value": channel?.tags ?? ""
            },
            {
                "type": "checkbox",
                "xs": 12,
                "fullWidth": true,
                "onChange": onChange,
                "name": "isActive",
                "text": "Is Active",
                "value": channel?.isActive
            },
            {
                "type": "container",
                "controls": [
                    {
                        "type": "button",
                        "xs": 6,
                        "text": "Close",
                        "onClick": close,
                    },
                    {
                        "type": "button",
                        "xs": 6,
                        "text": "Save",
                        "onClick": () => callback(channel),
                    }
                ],
                "xs": 12
            },
        ]
    }
    return (
        <Form spacing={channelForm.spacing} controls={channelForm.controls} />
    )
}
