import { GridContainer, GridItem } from "./GridComponents";
import createElement from "./Element"
const Form = (props) => {
    const { spacing, controls } = props
    const createGridItem = (element, index) => {
        return (
            <GridItem key={index} xs={element.xs} sm={element.sm} md={element.md} lg={element.lg}>
                {createElement(element)}
            </GridItem>
        )
    }
    return (
        <GridContainer spacing={spacing}>
            {controls.map((element, index) => (
                createGridItem(element, index)
            ))}
        </GridContainer>
    )
}

export default Form