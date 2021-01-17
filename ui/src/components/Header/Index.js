import React from 'react'
import ChevronLeftIcon from '@material-ui/icons/ChevronLeft';
import Drawer from '@material-ui/core/Drawer';
import AppBar from "@material-ui/core/AppBar";
import Toolbar from "@material-ui/core/Toolbar";
import Typography from "@material-ui/core/Typography"
import IconButton from '@material-ui/core/IconButton';
import MenuIcon from '@material-ui/icons/Menu';
import Divider from '@material-ui/core/Divider';
import clsx from 'clsx';
import List from '@material-ui/core/List';
import { Link as RouterLink } from "react-router-dom";
import ListItem from '@material-ui/core/ListItem';
import ListItemIcon from '@material-ui/core/ListItemIcon';
import ListItemText from '@material-ui/core/ListItemText';
import useStyles from "./styles";
import { Directions } from "../../config/Routes/Index";
import { useLocation } from 'react-router-dom'


export default function Header(props) {
    const { drawerClose, drawerOpen, open } = props;
    const classes = useStyles()
    const pathName = useLocation().pathname.replace("/", "");
    const [pageTitle, setPageTitle] = React.useState(pathName === "" ? "Dashboard" : pathName)
    return (
        <>
            <AppBar
                position="fixed"
                className={clsx(classes.appBar, {
                    [classes.appBarShift]: open,
                })}
            >
                <Toolbar>
                    <IconButton
                        color="inherit"
                        aria-label="open drawer"
                        onClick={drawerOpen}
                        edge="start"
                        className={clsx(classes.menuButton, open && classes.hide)}
                    >
                        <MenuIcon />
                    </IconButton>
                    <Typography variant="h6" noWrap>
                        {pageTitle}
                    </Typography>
                </Toolbar>
            </AppBar>
            <Drawer
                className={classes.drawer}
                variant="persistent"
                anchor="left"
                open={open}
                classes={{
                    paper: classes.drawerPaper,
                }}
            >
                <div className={classes.drawerHeader}>
                    <IconButton onClick={drawerClose}>
                        <ChevronLeftIcon />
                    </IconButton>
                </div>
                <Divider />
                <List>
                    {Directions.map((d, index) => (
                        <RouterLink key={index} className={classes.routerLink} onClick={() => setPageTitle(d.Text)} to={d.Path}>
                            <ListItem button >
                                <ListItemIcon> {d.Icon}</ListItemIcon>
                                <ListItemText primary={d.Text} />
                            </ListItem>
                        </RouterLink>
                    ))}
                </List>
            </Drawer>
        </>
    )
}
