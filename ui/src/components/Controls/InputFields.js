import { TextField, Button as MaterialButton, FormControlLabel, Checkbox, IconButton as IcnButton, ButtonGroup as BtnGroup } from "@material-ui/core";
import createElement from "./Element";

const TextBox = (props) => {
    const { fullWidth, onChange, value, name, placeHolder } = props
    return (<TextField variant="outlined" fullWidth={fullWidth} value={value} onChange={(e) => onChange(e, name)} placeholder={placeHolder}></TextField>)
}
const IconButton = (props) => {
    const { children, onClick } = props
    return (<IcnButton onClick={onClick}>{children}</IcnButton>)
}

const Image = (props) => {
    const { src, width, height, onClick } = props
    return (<img style={{ cursor: (onClick != null ? "pointer" : "default") }} src={src} width={width} height={height} alt="" onClick={onClick} />)
}
const CheckBox = (props) => {
    const { text, name, onChange, checked } = props
    return (<FormControlLabel label={text} control={<Checkbox onChange={(e) => onChange(e, name)} checked={checked}></Checkbox>}></FormControlLabel>)
}
const Button = (props) => {
    const { onClick, text } = props
    return (<MaterialButton onClick={() => onClick()}>{text}</MaterialButton>)
}
const ButtonGroup = (props) => {
    const { controls } = props
    return (
        <BtnGroup>
            {controls.map((element, index) => {
                if (element[element.condition] === true || element[element.condition] === undefined)
                    return createElement(element, index)
                else
                    return null
            })}
        </BtnGroup>
    )
}

export { TextBox, Image, CheckBox, Button, IconButton, ButtonGroup }