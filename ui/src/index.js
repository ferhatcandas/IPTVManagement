import React from 'react';
import ReactDOM from 'react-dom';
import './style/index.css';
import 'bootstrap/dist/css/bootstrap.css';

import ChannelsComponent from "./components/Channels.js";


ReactDOM.render(<ChannelsComponent />,
  document.getElementById('tvchannels')
);

