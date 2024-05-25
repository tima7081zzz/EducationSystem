import React from 'react';
import { Box, Typography } from '@mui/material';

interface CourseHeaderProps {
  name: string;
  description?: string;
  headerColor: string;
}

const CourseHeader: React.FC<CourseHeaderProps> = ({ name, description, headerColor }) => {
  return (
    <Box 
      sx={{ 
        backgroundColor: headerColor, 
        padding: '16px', 
        borderRadius: '8px', 
        mb: 3,
        textAlign: 'center'
      }}
    >
      <Typography variant="h4" component="h1" gutterBottom>
        {name}
      </Typography>
      <Typography variant="body1" component="p">
        {description}
      </Typography>
    </Box>
  );
};

export default CourseHeader;
