import { useState, useLayoutEffect } from "react";

// Returns the window size on every change
const useWindowSize = () => {
  const [size, setSize] = useState([0, 0]);

  useLayoutEffect(() => {
    const updateSize = () => {
      setSize([window.innerWidth, window.innerHeight]);
    };

    window.addEventListener("resize", updateSize);
    updateSize();

    return () => window.removeEventListener("resize", updateSize);
  }, []);

  return { windowSize: { width: size[0], height: size[1] } };
};

export default useWindowSize;
