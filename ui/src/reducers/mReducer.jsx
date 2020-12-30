const initialState = {
  tab: "tvChannels",
};
const mReducer = (state = initialState, action) => {
  if (action.type === "changeTab") {
    return { tab: action.tab };
  } else return state;
};
export default mReducer;
