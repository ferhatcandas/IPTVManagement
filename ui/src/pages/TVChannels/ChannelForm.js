import React, { useState } from 'react'
import { TextField, Grid, Checkbox, FormControlLabel, Button } from "@material-ui/core";

// const useStyles = makeStyles((theme) => ({
//     fieldSpacing: {
//         paddingTop: theme.spacing(1)
//     }
// }))
export default function ChannelForm(props) {
    const { data, callback } = props
    const [channel, setChannel] = useState(data);
    const onChange = (event) => {
        let copyOfChannel = Object.assign({}, channel);
        copyOfChannel[event.target.name] = event.target.type === "checkbox" ? event.target.checked : event.target.value
        setChannel(copyOfChannel)
    }

    return (
        <div>
            <Grid container spacing={2}>
                <Grid item xs={12}>
                    <Grid container>
                        <Grid xs={1} item>
                            <img src={channel?.logo} width={45} height={45} alt="" />
                        </Grid>
                        <Grid xs={11} item>
                            <TextField fullWidth={true} name="logo" onChange={(e) => onChange(e)} value={channel?.logo} variant="outlined" placeholder="Logo"></TextField>
                        </Grid>
                    </Grid>
                </Grid>
                <Grid item xs={12}>
                    <TextField fullWidth={true} variant="outlined" name="name" onChange={(e) => onChange(e)} value={channel?.name} placeholder="Channel Name"></TextField>
                </Grid>
                <Grid item xs={12} >
                    <TextField fullWidth={true} variant="outlined" name="category" onChange={(e) => onChange(e)} value={channel?.category} placeholder="Category"></TextField>
                </Grid>
                <Grid item xs={12}>
                    <TextField fullWidth={true} variant="outlined" name="country" onChange={(e) => onChange(e)} value={channel?.country} placeholder="Country"></TextField>
                </Grid>
                <Grid item xs={12}>
                    <TextField fullWidth={true} variant="outlined" name="language" onChange={(e) => onChange(e)} value={channel?.language} placeholder="Language"></TextField>
                </Grid>
                <Grid item xs={12}>
                    <TextField fullWidth={true} variant="outlined" name="stream" onChange={(e) => onChange(e)} value={channel?.stream} placeholder="Stream"></TextField>
                </Grid>
                <Grid item xs={12}>
                    <FormControlLabel label="Is Active" control={<Checkbox name="isActive" onChange={(e) => onChange(e)} checked={channel?.isActive}></Checkbox>}></FormControlLabel>
                </Grid>
                <Grid item xs={12}>
                    <Grid container>
                        <Grid xs={6} item>
                            <Button>Close</Button>
                        </Grid>
                        <Grid xs={6} item>
                            <Button onClick={() => callback(channel)}>Save</Button>
                        </Grid>
                    </Grid>
                </Grid>
            </Grid>

        </div>
    )
}
