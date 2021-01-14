import React from 'react'
import Typography from "@material-ui/core/Typography";
import useStyles from "./styles";

export default function Footer() {
    const classes = useStyles()
    return (
        <div className={classes.root}>
            <Typography variant="h6">Footer</Typography>
        </div>
    )
}
