import Grid from "@material-ui/core/Grid";
const GridContainer = (props) => {
    const { spacing, children } = props
    return (
        <Grid container spacing={spacing}>
            {children}
        </Grid>
    )
}
const GridItem = (props) => {
    const { children, xs, sm, md, lg } = props

    return (
        <Grid item xs={xs} sm={sm} md={md} lg={lg} >
            {children}
        </Grid >
    )
}

export { GridContainer, GridItem } 