import React from 'react'
import { Grid, Button, Typography } from "@material-ui/core";

export default function YesNoPopup(props) {
    const { content, callback,buttonText } = props
    return (
        <Grid container spacing={2}>
            <Grid xs={12} item>
                <Typography variant="body1" align="center">
                    {content}
                </Typography>
            </Grid>
            <Grid xs={12} item >
                <Button fullWidth={true} color="inherit" onClick={() => callback(true)}>{buttonText}</Button>
            </Grid>
        </Grid>
    )
}
