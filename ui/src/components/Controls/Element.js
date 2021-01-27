import { Button, CheckBox, Image, TextBox, IconButton, ButtonGroup } from "./InputFields";
import { Edit as EditIcon, FileCopy as CopyIcon, Delete as DeleteIcon } from "@material-ui/icons";
import Form from "./Form";
const createElement = (element, index) => {
    switch (element.type) {
        case "deleteButton":
            return <IconButton key={index} onClick={element.onClick}><DeleteIcon /></IconButton>
        case "copyButton":
            return <IconButton key={index} onClick={element.onClick}><CopyIcon /></IconButton>
        case "editButton":
            return <IconButton key={index} onClick={element.onClick}><EditIcon /></IconButton>
        case "button":
            return <Button key={index} text={element.text} onClick={element.onClick} />
        case "checkbox":
            return <CheckBox key={index} text={element.text} onChange={element.onChange} name={element.name} checked={element.value} />
        case "image":
            return <Image onClick={element.onClick} key={index} src={element.value} width={element.width} height={element.height} />
        case "textbox":
            return <TextBox key={index} value={element.value} fullWidth={element.fullWidth} onChange={element.onChange} name={element.name} placeHolder={element.placeHolder} />
        case "buttonGroup":
            return <ButtonGroup controls={element.controls} />
        case "container":
            return <Form spacing={element.spacing} controls={element.controls} />
        case "text":
            return element.text
        default:
            return null
    }
}
export default createElement