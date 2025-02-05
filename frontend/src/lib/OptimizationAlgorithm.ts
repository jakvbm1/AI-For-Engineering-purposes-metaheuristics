export interface OptimizationAlgorithm {
  name: string;
  paramInfo: ParamInfo[];
}

export interface ParamInfo {
  name: string;
  description: string;
  upperBoundary: number;
  lowerBoundary: number;
  defaultValue: number;
}