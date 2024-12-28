import { ChevronsUpDown } from "lucide-react";
import { Button } from "./ui/button";
import { Popover, PopoverContent, PopoverTrigger } from "./ui/popover";
import { Checkbox } from "./ui/checkbox";
import { Label } from "./ui/label";

interface Props {
  placeholder: string;
  options: string[];
  checkedOptions: string[];
  onCheck: (o: string[]) => void;
}

export function MultiSelect({ placeholder, options, checkedOptions, onCheck }: Props) {
  const checkboxes = options.map((option) => {
    return (
      <Label key={option} className="flex px-4 py-3 gap-4 border-b transition-colors hover:bg-muted/50">
        <Checkbox
          checked={checkedOptions.includes(option)}
          onCheckedChange={(checked) => {
            return checked
              ? onCheck([...checkedOptions, option])
              : onCheck(checkedOptions.filter((v) => v !== option))
          }}
        />
        {option}
      </Label>
    )
  });

  return (
    <Popover>
      <PopoverTrigger asChild>
        <Button variant='outline' className="w-full justify-between">
          {checkedOptions.length ? checkedOptions.join(', ') : placeholder}
          <ChevronsUpDown className="opacity-50" />
        </Button>
      </PopoverTrigger>
      <PopoverContent className="p-0 w-[var(--radix-popover-trigger-width)] max-h-[var(--radix-popover-content-available-height)]">
        {checkboxes}
      </PopoverContent>
    </Popover>
  )
}