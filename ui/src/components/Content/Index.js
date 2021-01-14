import React from 'react'
import useStyles from "./styles";
import clsx from 'clsx';



export default function Content(props) {

    const classes = useStyles();
    const { open, component } = props
    return (
        <main className={clsx(classes.content, {
            [classes.contentShift]: open,
        })}
        >
            <div className={classes.conentMarginTop} />
            {component ?? "test"}

        </main>
    )
}
