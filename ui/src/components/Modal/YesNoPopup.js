import React from 'react'
import Button from "@material-ui/core/Button";
export default function YesNoPopup(props) {
    const { content, callback } = props
    return (
        <div>
            <div>{content}</div>
            <Button onClick={() => callback(true)}>Yes</Button>
        </div>
    )
}
