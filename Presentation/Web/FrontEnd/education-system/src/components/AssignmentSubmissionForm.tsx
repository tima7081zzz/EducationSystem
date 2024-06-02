import React, { useState } from 'react';
import { Box, Button, TextField, Typography, Alert } from '@mui/material';

const MAX_FILE_SIZE = 10 * 1024 * 1024; // 10MB in bytes
const MAX_TEXT_LENGTH = 300;

const AssignmentSubmissionForm: React.FC = () => {
  const [submissionText, setSubmissionText] = useState('');
  const [submissionFile, setSubmissionFile] = useState<File | null>(null);
  const [errors, setErrors] = useState<{ text?: string; file?: string }>({});

  const handleTextChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const newText = event.target.value;
    if (newText.length > MAX_TEXT_LENGTH) {
      setErrors(prev => ({ ...prev, text: 'Submission text cannot exceed 300 characters' }));
    } else {
      setErrors(prev => ({ ...prev, text: undefined }));
    }
    setSubmissionText(newText);
  };

  const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    if (event.target.files && event.target.files[0]) {
      const file = event.target.files[0];
      if (file.size > MAX_FILE_SIZE) {
        setErrors(prev => ({ ...prev, file: 'File size cannot exceed 10MB' }));
      } else {
        setErrors(prev => ({ ...prev, file: undefined }));
        setSubmissionFile(file);
      }
    }
  };

  const handleSubmit = (event: React.FormEvent) => {
    event.preventDefault();
    if (!errors.text && !errors.file) {
      // Handle the submission logic here
      console.log('Submitted text:', submissionText);
      if (submissionFile) {
        console.log('Submitted file:', submissionFile.name);
      }
    }
  };

  return (
    <Box component="form" onSubmit={handleSubmit} mt={4}>
      <Typography variant="h6" gutterBottom>
        Submit Assignment
      </Typography>
      <TextField
        label="Submission Text"
        multiline
        rows={4}
        variant="outlined"
        fullWidth
        value={submissionText}
        onChange={handleTextChange}
        error={!!errors.text}
        helperText={errors.text}
        margin="normal"
      />
      <input type="file" onChange={handleFileChange} />
      {errors.file && <Alert severity="error">{errors.file}</Alert>}
      <Box mt={2}>
        <Button variant="contained" color="primary" type="submit" disabled={!!errors.text || !!errors.file}>
          Submit
        </Button>
      </Box>
    </Box>
  );
};

export default AssignmentSubmissionForm;
