/* eslint-disable react/jsx-key */
import React, { lazy } from 'react'
import { Route, Switch } from 'react-router-dom'
import DashboardIcon from "@material-ui/icons/Dashboard";
import TvIcon from "@material-ui/icons/Tv";
import CastConnectedIcon from "@material-ui/icons/CastConnected";
// import Dashboard from "../../pages/Dashboard";
// import TVChannels from "../../pages/TVChannels/Index";
// import Integrations from "../../pages/Integrations/Index";

const Dashboard = lazy(() => import('../../pages/Dashboard'))
const TVChannels = lazy(() => import('../../pages/TVChannels/Index'))
const Integrations = lazy(() => import('../../pages/Integrations/Index'))




export const RouteController = (() => {
    return (
        <Switch>
            <Route key={-1} path="/" exact component={Dashboard} />,
            {Directions.map((direction, index) => (
                <Route key={index} path={direction.Path} exact component={direction.Component} />
            ))}
        </Switch>
    )
})



export const Directions = [
    {
        "Text": "Dashboard",
        "Icon": <DashboardIcon />,
        "Path": "/",
        "Component": Dashboard
    },
    {
        "Text": "Channels",
        "Icon": <TvIcon />,
        "Path": "/Channels",
        "Component": TVChannels
    },
    {
        "Text": "Integrations",
        "Icon": <CastConnectedIcon />,
        "Path": "/Integrations",
        "Component": Integrations
    },
]