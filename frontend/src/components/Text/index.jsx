import PropTypes from "prop-types";
import cn from "classnames";

import { colorClasses, textTypes } from "src/app-globals";

import styles from "./styles.module.scss";

const identifyElement = (type) => {
  switch (type) {
    case "heading___xxl":
    case "heading___xl":
    case "heading___lg":
      return "h1";
    case "heading___md":
      return "h2";
    case "heading___sm":
      return "h3";
    case "heading___xs":
      return "h4";
    case "heading___xxs":
      return "h5";
    case "heading___xxxs":
      return "h6";
    case "body___lg":
    case "body___md":
    case "body___sm":
    case "body___xs":
      return "p";
    default:
      return "span";
  }
};

const Text = ({ children, className, type, colorClass }) => {
  const Element = identifyElement(type);

  return (
    <Element
      className={cn(
        className,
        styles[`Text___${type}`],
        styles[`Text___${colorClass}`]
      )}
    >
      {children}
    </Element>
  );
};

Text.defaultProps = {
  className: null,
  children: null,
  colorClass: colorClasses.NEUTRAL["700"],
  type: textTypes.BODY.MD,
};

Text.propTypes = {
  children: PropTypes.node,
  className: PropTypes.string,
  colorClass: PropTypes.oneOf([
    colorClasses.NEUTRAL["900"],
    colorClasses.NEUTRAL["800"],
    colorClasses.NEUTRAL["700"],
    colorClasses.NEUTRAL["600"],
    colorClasses.NEUTRAL["500"],
    colorClasses.NEUTRAL["400"],
    colorClasses.NEUTRAL["300"],
    colorClasses.NEUTRAL["200"],
    colorClasses.NEUTRAL["100"],
    colorClasses.NEUTRAL["50"],
    colorClasses.NEUTRAL["0"],
    colorClasses.VIOLET["400"],
    colorClasses.VIOLET["300"],
    colorClasses.VIOLET["200"],
    colorClasses.VIOLET["100"],
    colorClasses.BLUE["400"],
    colorClasses.BLUE["300"],
    colorClasses.BLUE["200"],
    colorClasses.BLUE["100"],
    colorClasses.RED["400"],
    colorClasses.RED["300"],
    colorClasses.RED["200"],
    colorClasses.RED["100"],
    colorClasses.GREEN["400"],
    colorClasses.GREEN["300"],
    colorClasses.GREEN["200"],
    colorClasses.GREEN["100"],
    colorClasses.YELLOW["400"],
    colorClasses.YELLOW["300"],
    colorClasses.YELLOW["200"],
    colorClasses.YELLOW["100"],
  ]),
  type: PropTypes.oneOf([
    textTypes.HEADING.XXL,
    textTypes.HEADING.XL,
    textTypes.HEADING.LG,
    textTypes.HEADING.MD,
    textTypes.HEADING.SM,
    textTypes.HEADING.XS,
    textTypes.HEADING.XXS,
    textTypes.HEADING.XXXS,
    textTypes.BODY.LG,
    textTypes.BODY.MD,
    textTypes.BODY.SM,
    textTypes.BODY.XS,
    textTypes.SPAN.LG,
    textTypes.SPAN.MD,
    textTypes.SPAN.SM,
    textTypes.SPAN.XS,
  ]),
};

export default Text;
