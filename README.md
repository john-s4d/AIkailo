# AIkailo

AIkailo was an early attempt at building a modular learning system, worked on in 2020, and represents the first iteration of what later became Agience.

The project explores a message-driven architecture for training and running a shared intelligence core across multiple domains. External modules could publish structured input and training data through RabbitMQ, while the central service encoded those signals into a graph-backed spiking neural network persisted in Neo4j.

Rather than targeting a single use case, AIkailo was designed as a general substrate for learning associations between inputs and outputs across different modules such as interaction, MNIST, tic-tac-toe, and stock trading. The codebase experiments with neurons, synapses, synchronization, persistent graph storage, and a lightweight adapter layer for external providers.

This repository is incomplete and should be treated as an archival prototype. Important parts of the neural training logic, normalization, and graph operations were never fully implemented, but the overall design captures an ambitious early attempt at a reusable cognitive architecture.

## Concepts explored

- message-driven AI service architecture
- RabbitMQ-based external training and input pipelines
- Neo4j-backed persistent graph representation
- spiking neural network experimentation
- modular learning across multiple external domains
- early cognitive architecture / shared intelligence runtime design

## Status

Archival prototype preserved for historical interest.
