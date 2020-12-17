import React from 'react';
import ReactDOM from 'react-dom';
import App from './App.js';
// import { createStore } from "redux";
// function foodReducer(state = 'elma', action) {
//   switch (action.type) {
//     case "UPDATE_FOOD":
//       return action.payload;
//     default:
//       return state;
//   }
// }
// const store = createStore(foodReducer);
// console.log(store.getState());
// const updateFoodDatas = {
//   type: "UPDATE_FOOD",
//   payload: "armut"
// }

// store.dispatch(updateFoodDatas);
// console.log(store.getState());

ReactDOM.render(<App />, document.getElementById('app'));
