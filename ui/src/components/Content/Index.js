import React, { Suspense } from 'react'
import useStyles from "./styles";
import clsx from 'clsx';
import { RouteController as Containers } from "../../config/Routes/Index";
import Waves from "@material-ui/icons/Waves";

export default function Content(props) {
    const classes = useStyles();
    const { open } = props
    return (

        <main className={clsx(classes.content, {
            [classes.contentShift]: open,
        })}
        >
            <div className={classes.conentMarginTop} />


            <Suspense fallback={<Waves className={classes.loading} />}>
                <Containers />
            </Suspense>


        </main>
    )
}
