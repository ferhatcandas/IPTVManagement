import { createMuiTheme } from '@material-ui/core/styles';

const theme = createMuiTheme({
    palette: {
        primary: {
            main: "#333",
        },
        secondary: {
            main: "rgba(255, 255, 255, 0.7)",
        },
        type: "dark"
    },
});
export default theme;