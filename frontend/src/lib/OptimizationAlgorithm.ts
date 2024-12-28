export interface OptimizationAlgorithm {
  name: string;
  paramsInfo: ParamInfo[];
}

export interface ParamInfo {
  name: string;
  description: string;
  upperBoundary: number;
  lowerBoundary: number;
  defaultValue: number;
}