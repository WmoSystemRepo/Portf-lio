export interface ImportHistory {
  id: string;            
  templateName: string;  
  fileName: string;      
  attemptDate: Date;    
  success: boolean;     
  pendencias: {         
    row: number;         
    column: string;      
    message: string;    
  }[];                   
}
