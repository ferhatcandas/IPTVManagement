import { makeStyles } from "@material-ui//core/styles";
import { drawerWidth } from '../Header/styles'
export default makeStyles((theme) => ({
    conentMarginTop: {
        ...theme.mixins.toolbar,
    },
    content: {
        display: "inline",
        overflowX: "auto",
        flexGrow: 1,
        padding: theme.spacing(3),
        transition: theme.transitions.create('margin', {
            easing: theme.transitions.easing.sharp,
            duration: theme.transitions.duration.leavingScreen,
        }),
        marginLeft: -drawerWidth,
    },
    contentShift: {
        transition: theme.transitions.create('margin', {
            easing: theme.transitions.easing.easeOut,
            duration: theme.transitions.duration.enteringScreen,
        }),
        marginLeft: 0,
    },
    loading: {
        position: "fixed", /* or absolute */
        top: "50%",
        left: "50%"
    }
}))

