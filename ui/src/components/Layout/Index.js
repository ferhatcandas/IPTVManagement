
import React from 'react'
import useStyles from "./styles";
import Header from "../Header/Index";
import Content from "../Content/Index";
import Grid from "@material-ui/core/Grid";

export default function Layout() {
    const classes = useStyles();
    const [open, setOpen] = React.useState(false);
    const handleDrawerOpen = () => {
        setOpen(true);
    };

    const handleDrawerClose = () => {
        setOpen(false);
    };
    return (
        <Grid container>
            <Grid xs={12} item className={classes.root}>
                <Header drawerClose={handleDrawerClose} drawerOpen={handleDrawerOpen} open={open} />
                <Content open={open} />
            </Grid>
        </Grid>
    )
}
